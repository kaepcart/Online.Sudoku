function Hub() {


    var chat = $.connection.sudokuHub;
    // Открываем соединение
    $.connection.hub.start().done(function () {
    });

    chat.client.InsertNumber = function (obj) {
        console.log(obj);
        alert();
        
    };


}