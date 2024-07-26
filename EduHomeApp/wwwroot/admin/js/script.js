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
                $(".tags").append(data)
                btn.parent().remove();
            }
            });
    });

    //course Delete start
    $(".delete-course").on("click", function (e) {
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
                const courseId = btn.attr("course-id");

                $.ajax({
                    url: `/adminarea/Course/Delete/${courseId}`,
                    method: "delete",
                    success: function () {
                        btn.parent().parent().remove();
                    },

                });
            }
        });
    });


    //courselanguage delete start

    $(".delete-courselanguage").on("click", function (e) {
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
                const languageId = btn.attr("language-id");

                $.ajax({
                    url: `/adminarea/CourseLanguage/Delete/${languageId}`,
                    method: "delete",
                    success: function () {
                        btn.parent().parent().remove();
                    },

                });
            }
        });
    });

    //category Delete start
    $(".delete-category").on("click", function (e) {
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
                const categoryId = btn.attr("category-id");

                $.ajax({
                    url: `/adminarea/Category/Delete/${categoryId}`,
                    method: "delete",
                    success: function () {
                        btn.parent().parent().remove();
                    },

                });
            }
        });
    });


    //remove Speaker from event start

    $(".removespeaker").on("click", function (e) {
        const eventId = $(this).attr("event-id");
        const speakerId = $(this).attr("speaker-id");
        const btn = $(this);
        $.ajax({
            url: `/adminarea/Event/RemoveSpeakers?eventId=${eventId}&speakerId=${speakerId}`,
            method: "delete",
            success: function (data) {
                $(".speakers").append(data)
                btn.parent().remove();
            }
        });
    });

    //event delete start

    $(".delete-event").on("click", function (e) {
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
                const eventId = btn.attr("event-id");

                $.ajax({
                    url: `/adminarea/Event/Delete/${eventId}`,
                    method: "delete",
                    success: function () {
                        btn.parent().parent().remove();
                    },

                });
            }
        });
    });

    //Speaker delete start

    $(".delete-speaker").on("click", function (e) {
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
                const speakerId = btn.attr("speaker-id");

                $.ajax({
                    url: `/adminarea/Speaker/Delete/${speakerId}`,
                    method: "delete",
                    success: function () {
                        btn.parent().parent().remove();
                    },

                });
            }
        });
    });


    //Notice Board delete start

    $(".delete-notice").on("click", function (e) {
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
                const noticeId = btn.attr("notice-id");

                $.ajax({
                    url: `/adminarea/NoticeBoard/Delete/${noticeId}`,
                    method: "delete",
                    success: function () {
                        btn.parent().parent().remove();
                    },

                });
            }
        });
    });

    //Slider  delete start

    $(".delete-slider").on("click", function (e) {
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
                const sliderId = btn.attr("slider-id");

                $.ajax({
                    url: `/adminarea/Slider/Delete/${sliderId}`,
                    method: "delete",
                    success: function () {
                        btn.parent().parent().remove();
                    },

                });
            }
        });
    });








});