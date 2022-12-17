
function addRow(aJson){
    var existingValue = document.getElementById("unitListBody").innerHTML;
    document.getElementById("unitListBody").innerHTML = existingValue + "<tr><td>" + aJson.id + "</td><td> " + aJson.unitType + "</td><td> "+ aJson.quantity + " </td></tr> " ;
}

function refreshTable() {
    document.getElementById("debug").innerHTML = "";
    let xhr = new XMLHttpRequest();
    xhr.open("GET", "http://localhost:5296/Unit", true);

    xhr.onreadystatechange = () => {
        if (xhr.readyState == XMLHttpRequest.DONE && xhr.status == 200) {
            var responseStruct = JSON.parse(xhr.response);
            document.getElementById("unitListBody").innerHTML = "";
            responseStruct.forEach(addRow);
        }
    };
    xhr.send();
}

function addunitRequest() {
    document.getElementById("debug").innerHTML = "";

    var unit = {};
    unit.Id = 0;
    unit.unitType= document.getElementById("unitType").value;
    unit.Quantity = document.getElementById("Quantity").value;

    let xhr = new XMLHttpRequest();
    xhr.open("POST", "http://localhost:5296/Unit", true);
    xhr.setRequestHeader("accept", "text/plain");
    xhr.setRequestHeader("Content-Type", "application/json");
   

    xhr.onreadystatechange = () => {
        if (xhr.readyState == XMLHttpRequest.DONE) {
            if (xhr.status == 200) {
                var responseStruct = JSON.parse(xhr.response);
                var existingValue = document.getElementById("unitListBody").innerHTML;
                document.getElementById("debug").innerHTML =  " added";
                document.getElementById("unitListBody").innerHTML = existingValue + "<tr><td>" + responseStruct.id + "</td><td>" + responseStruct.unitType + "</td><td> " + responseStruct.quantity + "</td></tr>" ;
            }
            else if (xhr.status == 204) {
                document.getElementById("debug").innerHTML = "Not Added. unitName already exist!";
            }
        }
    };
    var data = JSON.stringify(unit);

    xhr.send(data);
}

document.getElementById("unitAdd").addEventListener("click", function () { addunitRequest() });
document.getElementById("unitRefresh").addEventListener("click", function () { refreshTable() });