using System.Collections.Generic;
using AspNetAuth.Shared.Classes.Response;

namespace AspNetAuth.WebApp.ViewModels
{
    public class UsersListViewModel : BaseViewModel
    {
        public List<UserDto> Users { get; set; }
    }
}