using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Keycloak.Net;
using UlmApi.Domain.Dtos;
using UlmApi.Domain.Entities;
using UlmApi.Domain.Entities.Enums;
using UlmApi.Domain.Interfaces;
using UlmApi.Domain.Models;
using UlmApi.Domain.Models.Queries;

namespace UlmApi.Service.Services
{
    public class UserService : IUserService
    {
        private readonly ISolutionRepository _solutionRepository;
        private readonly KeycloakClient _keycloakClient;
        private readonly AuthenticatedUserService _authenticatedUserService;
        private readonly string REALM = Environment.GetEnvironmentVariable("KEYCLOAK_REALM") ?? "ulm";
        private readonly string CLIENT = Environment.GetEnvironmentVariable("KEYCLOAK_CLIENT") ?? "ulm-frontend";
        private readonly string USERNAME = Environment.GetEnvironmentVariable("KEYCLOAK_USERNAME") ?? "admin";
        private readonly string PASSWORD = Environment.GetEnvironmentVariable("KEYCLOAK_PASSWORD") ?? "admin";
        private readonly string KEYCLOAK_URL = Environment.GetEnvironmentVariable("KEYCLOAK_URL") ?? "http://localhost:8080";

        public UserService(ISolutionRepository solutionRepository, IMapper mapper, AuthenticatedUserService authenticatedUserService)
        {
            _solutionRepository = solutionRepository;
            _keycloakClient = new KeycloakClient(KEYCLOAK_URL, USERNAME, PASSWORD);
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<UserDto> GetOwnerBySolutionId(int solutionId)
        {
            var solution = await _solutionRepository.Select(solutionId);
            var owner = await _keycloakClient.GetUserAsync(REALM, solution.OwnerId);
            if (owner == null)
                return null;
            
            return new UserDto(owner, "OWNER");
        }

        public async Task<UsersListDto> GetUsers(GetUsersQuery queryParams)
        {
            var client = (await _keycloakClient.GetClientsAsync(REALM, CLIENT)).FirstOrDefault();
            var roles = await _keycloakClient.GetRolesAsync(REALM, client.Id);
            var data = new List<UserDto>();
            var users = (await _keycloakClient.GetUsersAsync(REALM)).Where(u => u.UserName != USERNAME);

            foreach (var user in users)
            {
                var userRoles = await _keycloakClient.GetClientRoleMappingsForUserAsync(REALM, user.Id, client.Id);
                data.Add(new UserDto(user, GetHighestRole(userRoles)));
            }

            if (!String.IsNullOrEmpty(queryParams.Term))
            {
                data = data.Where(u => u.FirstName.ToLower()
                           .Contains(queryParams.Term.ToLower())
                           || u.LastName.ToLower()
                           .Contains(queryParams.Term.ToLower())
                           || u.Email.ToLower()
                           .Contains(queryParams.Term.ToLower())
                           || u.Role.ToLower()
                           .Contains(queryParams.Term.ToLower()))
                           .ToList();
            }

            var totalPages = (int) Math.Ceiling((decimal) data.Count() / queryParams.Limit);

            if (queryParams.Pagineted)
                data = ApplyOrdering(data.AsQueryable(), queryParams.OrderBy)
                       .Skip(queryParams.Page * queryParams.Limit)
                       .Take(queryParams.Limit)
                       .ToList();

            return new UsersListDto
            {
                TotalPages = totalPages,
                Data = data,
                CurrentPage = queryParams.Page,
                ItemsPerPage = queryParams.Limit,
            };
        }

        public async Task<List<UserDto>> GetOwnersWithoutSolution()
        {
            var client = await _keycloakClient.GetClientsAsync(REALM, CLIENT);
            var owners = await _keycloakClient.GetUsersWithRoleNameAsync(REALM, client.FirstOrDefault().Id, "OWNER");
            var solutions = await _solutionRepository.GetSolutions();

            foreach (var owner in owners)
            {
                var ownerSolution = solutions.Where(s => s.OwnerId == owner.Id).FirstOrDefault();
                if (ownerSolution != null)
                    owners = owners.Where(o => o.Id != owner.Id);            
            }

            return owners.Select(o => new UserDto(o, "OWNER")).ToList();
        }

        public async Task<UserDto> GetUserById(string id)
        {
            var users = await GetUsers(new GetUsersQuery(pagineted: false));
            var user = users.Data.Where(u => u.Id == id).FirstOrDefault();
            if (user == null)
                return null;

            return user;
        }

        public async Task<Keycloak.Net.Models.Users.User> GetUserKeycloak(string userId)
        {
            try
            {
                var user = await _keycloakClient.GetUserAsync(REALM, userId);
                return user;
            }
            catch (System.Exception)
            {
                return null;
            } 
        }

        public async Task DisableUser(Keycloak.Net.Models.Users.User userKeycloak)
        {
            userKeycloak.Enabled = false;
            await _keycloakClient.UpdateUserAsync(REALM, userKeycloak.Id, userKeycloak);
        }

        public async Task EnableUser(Keycloak.Net.Models.Users.User userKeycloak)
        {
            userKeycloak.Enabled = true;
            await _keycloakClient.UpdateUserAsync(REALM, userKeycloak.Id, userKeycloak);
        }

        public async Task UpdateUserRole(string userId, string role)
        {
            var clientId = (await _keycloakClient.GetClientsAsync(REALM, CLIENT)).FirstOrDefault()?.Id;
            
            var rolesAlreadyAssociatedWithUser = await _keycloakClient.GetClientRoleMappingsForUserAsync(REALM, userId, clientId);
            await _keycloakClient.DeleteClientRoleMappingsFromUserAsync(REALM, userId, clientId, rolesAlreadyAssociatedWithUser);
            
            var roleKeycloak = (await _keycloakClient.GetRolesAsync(REALM, clientId)).Where(r => r.Name == role);
            await _keycloakClient.AddClientRoleMappingsToUserAsync(REALM, userId, clientId, roleKeycloak);
        }

        public List<GenericEnumModel> GetAvailableRoles()
        {
            var roles = new List<GenericEnumModel>();
            var enumValues = Enum.GetValues(typeof(Role)).OfType<Role>().ToList();
            int i = 0;

            foreach (var item in enumValues)
            {
                roles.Add(new GenericEnumModel
                {
                    Value = i,
                    ValueString = item.ToString()
                });
                i++;
            }

            return roles;
        }

        private IQueryable<UserDto> ApplyOrdering(IQueryable<UserDto> query, string orderBy)
        {
            switch (orderBy)
            {
                case "role_desc":
                    query = query.OrderByDescending(u => u.Role);
                    break;

                case "role":
                    query = query.OrderBy(u => u.Role);
                    break;

                case "email_desc":
                    query = query.OrderByDescending(u => u.Email);
                    break;

                case "email":
                    query = query.OrderBy(u => u.Email);
                    break;

                case "fullName_desc":
                    query = query.OrderByDescending(u => u.FullName);
                    break;

                case "fullName":
                    query = query.OrderBy(u => u.FullName);
                    break;

                default:
                    query = query.OrderBy(u => u.Id);
                    break;
            }

            return query;
        }

        private string GetHighestRole(IEnumerable<Keycloak.Net.Models.Roles.Role> roles)
        {
            var rolesValues = roles.Select(r => r.Name).ToList();

            if (rolesValues.Contains("ADMIN"))
                return "ADMIN";

            if (rolesValues.Contains("OWNER"))
                return "OWNER";

            return "REQUESTER";
        }

        public async Task ChangePassword(ChangePasswordModel model)
        {
            await _keycloakClient.ResetUserPasswordAsync(REALM, _authenticatedUserService.Id, model.Password, false);
        }

        public async Task<bool> UserAlreadyOwnsASolution(string userId)
        {
            var solution = await _solutionRepository.GetSolutionByOwnerId(userId);
            return solution != null;
        }
        
    }
}
