
function addRow(aJson){
    var existingValue = document.getElementById("marketListBody").innerHTML;
    document.getElementById("maketListBody").innerHTML = existingValue + "<tr><td>" + aJson.id + "</td><td> " + aJson.marketName + "</td><td> "+ aJson.exchangeId + " </td></tr> " ;
}


function refreshTable() {
    document.getElementById("debug").innerHTML = "";
    let xhr = new XMLHttpRequest();
    xhr.open("GET", "https://localhost:5296/api/Market", true);

    xhr.onreadystatechange = () => {
        if (xhr.readyState == XMLHttpRequest.DONE && xhr.status == 200) {
            var responseStruct = JSON.parse(xhr.response);
            document.getElementById("marketListBody").innerHTML = "";
            responseStruct.forEach(addRow);
        }
    };
    xhr.send();
}
document.getElementById("marketRefresh").addEventListener("click", function () { refreshTable() });
