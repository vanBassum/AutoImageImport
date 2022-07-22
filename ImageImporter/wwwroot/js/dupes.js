// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/duplicateshub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

async function start() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};

connection.onclose(async () => {
    await start();
});

start();


connection.on("Removed", function (message) {
    var data = JSON.parse(message);
    var obj = $("#" + data);
    obj.remove();
});

$(function () {
    $(this).find("*.deletePicture").each(function (index, value) {
        var obj = $(this);
        
        obj.click(function (e) {
            var id = obj.attr('picid');
            connection.send("DeletePicture", id).catch(function (err) {
                return console.error(err.toString());
            });
        });
    });
});



