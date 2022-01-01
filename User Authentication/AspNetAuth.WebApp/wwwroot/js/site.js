// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const GlobalVars = {
    isAuthenticated: false,
    userRoles: []
}

const Users = {
    IsInRole: (role) => {
        return GlobalVars.userRoles.findIndex(x => x === role) !== -1;
    }
}