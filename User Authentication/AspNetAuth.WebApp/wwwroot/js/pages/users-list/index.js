$(".block-unblock-btn").on("click", function (evt) {
    const $button = $(evt.target);
    const selectedUserId = $button.data("user-id");
    const currentUserActiveFlag = $button.data("user-active") === "True";
    
    if (confirm(`Are you sure want to ${currentUserActiveFlag ? "block" : "unblock"} this user?`)) {
        $("#set-user-status-form")
            .attr("action", `/Users/SetUserStatus/${selectedUserId}?active=${!currentUserActiveFlag}`)
            .submit()
    }
})