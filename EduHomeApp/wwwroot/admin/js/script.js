$(function () {

    //Blog Delete start
    $(".delete-blog").on("click", function (e) {
        const btn = $(this);
        Swal.fire({
            title: "Are you sure?",
            text: "You won't delete this!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Yes, delete it!"
        }).then((result) => {
            if (result.isConfirmed) {
                const blogid = $(this).attr("blog-id");
                $.ajax({
                    url: "/adminarea/blog/delete",
                    method: "delete",
                    data: { id: blogid },
                    success: function () {
                        btn.parent().parent().remove();
                    }
                });
            }
        });
    });

    //User Search start

    $(".usersearch").on("click", function (e) {

        const inputvalue = $(".checkuser:checked").val();
        const textinp = $(".searchinp").val();
                $.ajax({
                    url: "/adminarea/user/SearchUser",
                    method: "get",
                    data: { text: textinp, value: inputvalue },
                    success: function (datas) {
                        $(".userviewarea tr").remove();

                        $(".userviewarea").append(datas);
                    }
                });
          
    });

    //user delete start

    //Blog Delete start
    $(".deleteuser").on("click", function (e) {
        const btn = $(this);
        Swal.fire({
            title: "Are you sure?",
            text: "You won't delete this!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Yes, delete it!"
        }).then((result) => {
            if (result.isConfirmed) {
                const userid = btn.attr("id");
               
                $.ajax({
                    url: `/adminarea/User/Delete/${userid}`,
                    method: "delete",
                    success: function () {
                        btn.parent().parent().remove();
                    },
                    
                });
            }
        });
    });

    //remove tag start


    $(".removetag").on("click", function (e) {
        const coursId = $(this).attr("cours-id");
        const tagId = $(this).attr("tag-id");
        const btn = $(this);
        $.ajax({
            url: `/adminarea/Course/RemoveTags?courseId=${coursId}&tagId=${tagId}`,
            method: "delete",
            success: function (data) {
                $(".tags").append(datas)
                btn.parent().remove();
            }
            });
    });

























});