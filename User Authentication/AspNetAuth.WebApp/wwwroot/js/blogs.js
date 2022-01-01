
function loadBlogs() {
    $(".blog-list-container").html("<p>Loading...</p>");
    
    let getBlogsUrl = "";
    if (Users.IsInRole("User")) {
        getBlogsUrl = "/api/Blogs/My"
    } else {
        getBlogsUrl = "/api/Blogs"
    }
    
    $.getJSON(getBlogsUrl, function (data) {
        const $table = $(`
            <table class="table table-hover">
                <thead>
                    <tr>
                        <th scope="col">#</th>
                        <th scope="col">Title</th>
                        <th scope="col">Body</th>
                        <th scope="col">${Users.IsInRole("User") ? "Actions" : "Author"}</th>
                    </tr>
                </thead>
            </table>
        `);
        
        const $tbody = $("<tbody></tbody>");
        
        $.each(data, function (idx, item) {
            $tbody.append(`
                <tr>
                    <td><strong>${idx + 1}</strong></td>
                    <td><strong>${item.title}</strong></td>
                    <td>${item.body}</td>
                    <td>
                        ${Users.IsInRole("User") ? 
                            `<button data-blog-id="${item.id}" class="btn btn-outline-danger delete-blog-btn">Delete</button>` : 
                            `${item.user.displayName}`}
                    </td>
                </tr>
            `)
        })
        
        $table.append($tbody);
        $(".blog-list-container").empty().append($table);
        
        $(".delete-blog-btn").on("click", function (evt) {
            const $button = $(evt.target);
            const blogId = $button.data("blog-id");
            
            if (confirm("Are you sure want to delete this blog ?")) {
                $button.prop("disabled", true);
                $button.text("Deleting...");
                
                $.ajax("/api/Blogs/" + blogId, { method: "DELETE" })
                    .done(function (response, textStatus, jqXHR) {
                        loadBlogs()
                    })
                    .fail(function (jqXHR, statusText, error) {
                        const response = jqXHR.responseJSON;
                        if (response && response.message) {
                            alert(response.message)
                        }
                    })
                    .always(function () {
                        $button.prop('disabled', false);
                        $button.text("Delete");
                    })
            }
        })
    })
}

loadBlogs()