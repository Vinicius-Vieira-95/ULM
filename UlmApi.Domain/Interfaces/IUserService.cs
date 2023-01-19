using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using UlmApi.Domain.Attributes;
using UlmApi.Domain.Dtos;
using UlmApi.Domain.Entities;
using UlmApi.Domain.Models;
using UlmApi.Domain.Models.Queries;

namespace UlmApi.Domain.Interfaces
{
    public interface IUserService
    {
        [Log(Operation.QUERY, "User", "Returning owner by solutionId.")]
        Task<UserDto> GetOwnerBySolutionId(int solutionId);

        [Log(Operation.QUERY, "User", "Returning all users.")]
        Task<UsersListDto> GetUsers(GetUsersQuery queryParams);
        
        [Log(Operation.QUERY, "User", "Returning all owners without solution.")]
        Task<List<UserDto>> GetOwnersWithoutSolution();

        [Log(Operation.QUERY, "User", "Returning user by Id.")]
        Task<UserDto> GetUserById(string id);

        [Log(Operation.QUERY, "User", "Returning keycloak user.")]
        Task<Keycloak.Net.Models.Users.User> GetUserKeycloak(string userId);

        [Log(Operation.UPDATE, "User", "Disabling user.")]
        Task DisableUser(Keycloak.Net.Models.Users.User userKeycloak);
        
        [Log(Operation.UPDATE, "User", "Enabling user.")]
        Task EnableUser(Keycloak.Net.Models.Users.User userKeycloak);

        [Log(Operation.UPDATE, "User", "Update user role.")]
        Task UpdateUserRole(string userId, string role);

        [Log(Operation.QUERY, "User", "Returning availables roles.")]
        List<GenericEnumModel> GetAvailableRoles();

        [Log(Operation.UPDATE, "User", "Change user password.")]
        Task ChangePassword(ChangePasswordModel model);

        [Log(Operation.QUERY, "User", "Checking if the user owns a solution.")]
        Task<bool> UserAlreadyOwnsASolution(string userId);
    }
}
