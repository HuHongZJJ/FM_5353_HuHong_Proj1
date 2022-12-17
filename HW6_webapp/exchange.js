
function addRow(aJson){
    var existingValue = document.getElementById("exchangeListBody").innerHTML;
    document.getElementById("exchangeListBody").innerHTML = existingValue + "<tr><td>" + aJson.id + "</td><td> " + aJson.exchangeName + "</td><td> "+ aJson.exchangeSymbol + " </td></tr> " ;
}

function refreshTable() {
    document.getElementById("debug").innerHTML = "";
    let xhr = new XMLHttpRequest();
    xhr.open("GET", "http://localhost:5296/Exchange", true);

    xhr.onreadystatechange = () => {
        if (xhr.readyState == XMLHttpRequest.DONE && xhr.status == 200) {
            var responseStruct = JSON.parse(xhr.response);
            document.getElementById("exchangeListBody").innerHTML = "";
            responseStruct.forEach(addRow);
        }
    };
    xhr.send();
}

function addExchangeRequest() {
    document.getElementById("debug").innerHTML = "";

    var exchange = {};
    exchange.Id = 0;
    exchange.ExchangeName= document.getElementById("exchangeName").value;
    exchange.ExchangeSymbol = document.getElementById("exchangeSymbol").value;

    let xhr = new XMLHttpRequest();
    xhr.open("POST", "http://localhost:5296/Exchange", true);
    xhr.setRequestHeader("accept", "text/plain");
    xhr.setRequestHeader("Content-Type", "application/json");
   

    xhr.onreadystatechange = () => {
        if (xhr.readyState == XMLHttpRequest.DONE) {
            if (xhr.status == 200) {
                var responseStruct = JSON.parse(xhr.response);
                var existingValue = document.getElementById("exchangeListBody").innerHTML;
                document.getElementById("debug").innerHTML =  " added";
                document.getElementById("exchangeListBody").innerHTML = existingValue + "<tr><td>" + responseStruct.id + "</td><td>" + responseStruct.exchangeName + "</td><td> " + responseStruct.exchangeSymbol + "</td></tr>" ;
            }
            else if (xhr.status == 204) {
                document.getElementById("debug").innerHTML = "Not Added. ExchangeName already exist!";
            }
        }
    };
    var data = JSON.stringify(exchange);

    xhr.send(data);
}

document.getElementById("exchangeAdd").addEventListener("click", function () { addExchangeRequest() });
document.getElementById("exchangeRefresh").addEventListener("click", function () { refreshTable() });