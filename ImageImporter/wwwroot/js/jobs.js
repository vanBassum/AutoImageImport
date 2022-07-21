// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/jobshub")
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


connection.on("JobUpdate", function (message) {
    var data = JSON.parse(message);
    var obj = $("#" + data.Name);

    obj.find("#interval").text(data.Interval);
    obj.find("#lastExecution").text(data.LastExecution);
    obj.find("#nextExecution").text(data.NextExecution);
    obj.find("#duration").text(data.Duration);
    var prog = obj.find(".progress-bar");
    prog.attr('aria-valuenow', data.Progress);
    prog.attr('style', 'width:' + data.Progress + '%');
    prog.text(data.Progress + '%');
});

$(function () {
    $(this).find("*.runJob").each(function (index, value) {
        var obj = $(this);

        obj.click(function (e) {
            var name = obj.attr('jobKey');
            connection.send("RunJob", name).catch(function (err) {
                return console.error(err.toString());
            });
        });


    });

});


