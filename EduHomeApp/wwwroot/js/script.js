$(function () {
    //Subscribe js start
    $("#mc-embedded-subscribe").on("click", function (event) {
        event.preventDefault();

        const email = $("#mce-EMAIL").val(); 

        $.ajax({
            url: "/Subscribe/SubscribeEmail",
            method: "POST",
            data: { email: email }, 
            success: function () {
                Swal.fire({
                    icon: "success",
                    title: "THANK YOU",
                    timer: 1500
                });
            },
            error: function (xhr) {
                Swal.fire({
                    icon: "error",
                    title: "Oops...",
                    text: xhr.responseText
                });
            }
        });
    });
    //Subscribe js end

   // course search js start
    $(".search-btn").on("click", function (e) {

        const textinp = $(this).prev().val().trim();

        if (textinp !== "") {
            e.preventDefault();

            $.ajax({
                url: "/Courses/SearchCourse",
                method: "GET",
                data: { text: textinp },
                success: function (datas) {
                    $(".coursespage div").remove();
                    $(".coursespage").append(datas)
                },
                error: function (xhr) {

                }
            });
        }
    });

    //course search js end




});