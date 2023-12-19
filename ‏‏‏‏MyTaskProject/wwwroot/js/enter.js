﻿const url = '/User';

function Login() {
    sessionStorage.clear();
    var myHeaders = new Headers();
    const name = document.getElementById('name').value.trim();
    const password = document.getElementById('password').value.trim();

    myHeaders.append("Content-Type", "application/json");
    var raw = JSON.stringify({
        Name: name,
        Password: password
    })
    var requestOptions = {
        method: "POST",
        headers: myHeaders,
        body: raw,
        redirect: "follow",
    };

    fetch(`${url}/Login`, requestOptions)
        .then((response) => response.text())
        .then((result) => {
            if (result.includes("Unauthorized")) {
                alert("not exist!!")
            } else {
                token = result;
                //sessionStorage.setItem("name", name);
                //sessionStorage.setItem("password", password);
                sessionStorage.setItem("token", token)
                location.href = "task.html";

            }
        }).catch((error) => alert("error", error));

}