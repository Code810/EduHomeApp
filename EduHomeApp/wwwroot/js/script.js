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

   
    $(".send-message").on("click", function (event) {
        event.preventDefault();

        const messageText = $("#message-text").val();
        const messageSubject = $("#message-subject").val();
        const messageEmail = $("#message-email").val();
        const messageName = $("#message-name").val();

        if (!messageEmail || !messageName || !messageSubject || !messageText) {
            Swal.fire({
                icon: "warning",
                title: "Validation Error",
                text: "Please fill out all fields."
            });
            return;
        }

        Swal.fire({
            title: "Sending...",
            text: "Please wait.",
            allowOutsideClick: false,
            onBeforeOpen: () => {
                Swal.showLoading();
            }
        });

        $.ajax({
            url: "/Contact/SendMessage",
            method: "POST",
            data: { Email: messageEmail, FullName: messageName, Subject: messageSubject, MessageText: messageText },
            success: function (response) {
                Swal.close(); 

                if (response.success === false) {
                    Swal.fire({
                        icon: "error",
                        title: "Oops...",
                        text: response.message || "An error occurred."
                    });
                    return;
                }
                $("#ContactMessages").append(response);
                Swal.fire({
                    icon: "success",
                    title: "THANK YOU",
                    timer: 1500
                });
            },
            error: function (xhr) {
                Swal.close();
                Swal.fire({
                    icon: "error",
                    title: "Oops...",
                    text: xhr.responseText || "An error occurred."
                });
            }
        });
    });

    $(".remove-message").on("click", function () {
        var btn = $(this);
        var messageId = btn.attr("message-id");
        $.ajax({
            url: "/Contact/MessageChangeIsdelete",
            method: "POST",
            data: { id: messageId },
            success: function () {
                btn.parent().parent().remove();
            },
        });
           
    });
    











});