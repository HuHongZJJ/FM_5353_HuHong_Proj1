
function addRow(aJson){
    var existingValue = document.getElementById("rateCurveListBody").innerHTML;
    document.getElementById("rateCurveListBody").innerHTML = existingValue + "<tr><td>" + aJson.id + "</td><td> " + aJson.name + "</td><td> "+ aJson.symbol + " </td></tr> " ;
    
}

function refreshTable() {
    document.getElementById("debug").innerHTML = "";
    let xhr = new XMLHttpRequest();
    xhr.open("GET", "http://localhost:5296/RatesCurve", true);

    xhr.onreadystatechange = () => {
        if (xhr.readyState == XMLHttpRequest.DONE && xhr.status == 200) {
            var responseStruct = JSON.parse(xhr.response);
            document.getElementById("rateCurveListBody").innerHTML = "";
            responseStruct.forEach(addRow);
        }
    };
    xhr.send();
}

function addRateCurveRequest() {
    document.getElementById("debug").innerHTML = "";

    var rateCurve = {};
    rateCurve.Id = 0;
    rateCurve.Name= document.getElementById("rateCurveName").value;
    rateCurve.Symbol = document.getElementById("rateCurveSymbol").value;

    let xhr = new XMLHttpRequest();
    xhr.open("POST", "http://localhost:5296/RatesCurve", true);
    xhr.setRequestHeader("accept", "text/plain");
    xhr.setRequestHeader("Content-Type", "application/json");
   

    xhr.onreadystatechange = () => {
        if (xhr.readyState == XMLHttpRequest.DONE) {
            if (xhr.status == 200) {
                var responseStruct = JSON.parse(xhr.response);
                var existingValue = document.getElementById("rateCurveListBody").innerHTML;
                document.getElementById("debug").innerHTML =  " added";
                document.getElementById("rateCurveListBody").innerHTML = existingValue + "<tr><td>" + responseStruct.id + "</td><td>" + responseStruct.name + "</td><td> " + responseStruct.symbol + "</td></tr>" ;
            }
            else if (xhr.status == 204) {
                document.getElementById("debug").innerHTML = "Not Added. RateCurve already exist!";
            }
        }
    };
    var data = JSON.stringify(rateCurve);

    xhr.send(data);
}

document.getElementById("rateCurveAdd").addEventListener("click", function () { addRateCurveRequest() });
document.getElementById("rateCurveRefresh").addEventListener("click", function () { refreshTable() });