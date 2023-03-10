
$(function () {
    AfterLoad($(this));
});

function AfterLoad(loadedObj) {

    //loadedObj.find("*.Jobs").each(function (index, value) {
    //    var connection = new signalR.HubConnectionBuilder().withUrl("/jobHub").build();
    //    var obj = $(this);
    //    var exampleRow = obj.find("[idattr='-1']")
    //
    //    connection.on("ReceiveJobNew", function (jobInfo) {
    //        console.log("test");
    //        var row = exampleRow.clone();
    //        row.attr("hidden", false);
    //        obj.append(row);
    //
    //
    //        row.find(".row-parent").attr("data-bs-target", "#job" + jobInfo.name);
    //        row.find(".row-child").attr("id", "job" + jobInfo.name);
    //
    //        row.find("#name").html(jobInfo.name);
    //        row.find("#next").html(jobInfo.Started);
    //
    //
    //
    //
    //
    //        //            row.attr("idattr", jobInfo.);
    //        //
    //        //
    //        //
    //        //var button = row.find("#cancel")
    //        //button.click(function (e) {
    //        //    connection.invoke("CancelJob", jobId);
    //        //});
    //
    //    });
    //
    //    connection.start().then(function () {
    //        connection.invoke("GetJobs");
    //    }).catch(function (err) {
    //        return console.error(err.toString());
    //    });
    //
    //
    //});

}






