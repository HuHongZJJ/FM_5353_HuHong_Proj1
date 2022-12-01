const theForm = document.getElementById("theForm")

theForm.addEventListener("submit", async (e) => {
    //Prevent browser default behavior
    e.preventDefault();
    let form = e.currentTarget;
    let url = form.action;

    try {
        let formData = new FormData(form);
        
        let responseData = await postFormFieldsAsJson({ url, formData });
        
        let { serverDataResponse } = responseData;
        //console.log(serverDataResponse);
        console.log(responseData["price"]);
        
        document.getElementById("price").innerHTML = responseData["price"];
        document.getElementById("delta").innerHTML = responseData["delta"];
        document.getElementById("theta").innerHTML = responseData["theta"];
        document.getElementById("gamma").innerHTML = responseData["gamma"];
        document.getElementById("vega").innerHTML = responseData["vega"];
        document.getElementById("rho").innerHTML = responseData["rho"];
        document.getElementById("stdErrorNorm").innerHTML = responseData["stdErrorNorm"];
        document.getElementById("stdErrorReduce").innerHTML = responseData["stdErrorReduce"];
        
    
    
    }
    catch (error) {
        console.error(error);
    }
  });

async function postFormFieldsAsJson({ url, formData }) {
    let formDataObject = Object.fromEntries(formData.entries());
    formDataObject["IsThread"] =(formDataObject["IsThread"] === 'true');
    formDataObject["IsAntithetic"] =(formDataObject["IsAntithetic"] === 'true');
    formDataObject["IsDeltaCV"] =(formDataObject["IsDeltaCV"] === 'true');
    formDataObject["IsCall"] =(formDataObject["IsCall"] === 'true');

    let formDataJsonString = JSON.stringify(formDataObject);
    let fetchOptions = {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            Accept: "application/json",
          },
          body: formDataJsonString,
        };

        let res = await fetch(url, fetchOptions);
        if (!res.ok) {
            let error = await res.text();
            throw new Error(error);
        }
    return res.json();
}

