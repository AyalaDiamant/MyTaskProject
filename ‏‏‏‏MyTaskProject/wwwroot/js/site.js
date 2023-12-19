const uri = '/Task';
let tasks = [];
let Employee = [];

// GET Tasks
function getItems() {
    let token = sessionStorage.getItem("token");
    var headers = new Headers();
    headers.append("Authorization", "Bearer " + token);
    headers.append("Content-Type", "application/json");
    var requestOptions = {
        method: 'GET',
        headers: headers,
        redirect: 'follow'
    };

    fetch(uri, requestOptions)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.log('error', error));
}

getItems();

// GET Task by id
function getItemById() {
    let token = sessionStorage.getItem("token");
    const id = document.getElementById('get-item').value;
    var headers = new Headers();
    headers.append("Authorization", "Bearer " + token);
    headers.append("Content-Type", "application/json");
    var requestOptions = {
        method: 'GET',
        headers: headers,
        redirect: 'follow'
    };
    fetch(`${uri}/${id}`, requestOptions)
        .then(response => response.json())
        .then(data => {
            if (data.title == 'Not Found') { alert('Not Found!!'); }
            else { showItem(data); }
        }
        )
        .catch(error => console.error('Unable to get items.', error));
}

function showItem(data) {
    const descreption = document.getElementById('descreption');
    const status = document.getElementById('status');
    descreption.innerText = "Task descreption: " + data.descreption;
    status.innerText = "status: " + data.status;

}

// CREAT Task
function addItem() {
    let token = sessionStorage.getItem("token");
    const addDescreptionTextbox = document.getElementById('add-descreption');
    const item = {
        id: 0,
        descreption: addDescreptionTextbox.value.trim(),
        status: false,
        agentId: 123
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': "Bearer " + token
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            getItems(token);
            addDescreptionTextbox.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}


// DELATE Task
function deleteItem(id) {
    let token = sessionStorage.getItem("token");
    fetch(`${uri}/${id}`, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': "Bearer " + token
        }
    }).then(() => { getItems(token) })
        .catch(error => console.error('Unable to delete item.', error));
}

// EDIT Task
function displayEditForm(id) {
    const item = tasks.find(item => item.id === id);
    document.getElementById('edit-descreption').value = item.descreption;
    document.getElementById('edit-status').checked = item.status;
    document.getElementById('editForm').style.display = 'block';
    updateItem(item)
}

function updateItem(item1) {
    let token = sessionStorage.getItem("token");
    document.getElementById('save').onclick = () => {
        const item = {
            agentId: parseInt(item1.agentId),
            Id: parseInt(item1.id),
            status: document.getElementById('edit-status').checked,
            descreption: document.getElementById('edit-descreption').value.trim()
        };
        fetch(`${uri}/${item1.id}`, {
            method: 'PUT',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': "Bearer " + token
            },
            body: JSON.stringify(item)
        })
            .then(() => getItems(token))
            .catch(error => console.error('Unable to update item.', error));
        closeInput();
    }
}


function closeInput() {
    document.getElementById('editForm').style.display = 'none';
    document.getElementById('editFormEmployee').style.display = 'none';
}

function _displayCount(itemCount) {
    const descreption = (itemCount === 1) ? 'task' : 'task kinds';
    document.getElementById('counter').innerText = `${itemCount} ${descreption} `;
}

// שייך להצגת המשימות
function _displayItems(data) {
    const tBody = document.getElementById('tasks');
    tBody.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {
        let statusCheckbox = document.createElement('input');
        statusCheckbox.type = 'checkbox';
        statusCheckbox.disabled = true;
        statusCheckbox.checked = item.status;

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(statusCheckbox);

        let td2 = tr.insertCell(1);
        let textNode = document.createTextNode(item.descreption);
        td2.appendChild(textNode);

        let td3 = tr.insertCell(2);
        td3.appendChild(editButton);

        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });

    tasks = data;
}



/// Employee ///
const url = '/Employee';

// GET Employee
document.getElementById('getAll').onclick = () => {
    GetAllEmployees()
}

function GetAllEmployees() {
    let token = sessionStorage.getItem("token");
    var headers = new Headers();
    headers.append("Authorization", "Bearer " + token);
    headers.append("Content-Type", "application/json");

    var requestOptions = {
        method: 'GET',
        headers: headers,
        headers: headers,
        redirect: 'follow',
    };

    fetch(url, requestOptions)
        .then(response => response.json())
        .then(data => _displayUsers(data))
        .catch(error => { console.log('error', error), alert('Not Authorized!!') });

}

// הצגת העובדים
function _displayUsers(data) {
    document.getElementById('manager').style.display = 'block';
    const tBody = document.getElementById('Users');
    tBody.innerHTML = '';
    const button = document.createElement('button');

    data.forEach(user => {

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditFormEmployee(${user.userId})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = '❌';
        deleteButton.setAttribute('onclick', `deleteUser(${user.userId})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        let textNode1 = document.createTextNode(user.userId);
        td1.appendChild(textNode1);

        let td2 = tr.insertCell(1);
        let textNode2 = document.createTextNode(user.name);
        td2.appendChild(textNode2);

        let td3 = tr.insertCell(2);
        let textNode3 = document.createTextNode(user.password);
        td3.appendChild(textNode3);

        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);

        let td5 = tr.insertCell(4);
        td5.appendChild(editButton);
    });

    Employee = data;
}

// GET Employee by id
function getEmployeeById() {
    let token = sessionStorage.getItem("token");
    const id = document.getElementById("get-employee").value;
    var headers = new Headers();
    headers.append("Authorization", "Bearer " + token);
    headers.append("Content-Type", "application/json");
    var requestOptions = {
        method: 'GET',
        headers: headers,
        redirect: 'follow'
    };
    fetch(`${url}/${id}`, requestOptions)
        .then(response => response.json())
        .then(data => {
            if (data.title == 'Not Found') { alert('Not Found!!'); }
            else { showEmployee(data); }
        }
        )
        .catch(error => console.error('Unable to get items.', error));
}

function showEmployee(data) {
    const Employee = document.getElementById('Employee');
    Employee.innerHTML = "";
    Employee.innerHTML = "EmployeeID: " + data.userId;
    Employee.innerHTML += " ";
    Employee.innerHTML += "EmployeeName: " + data.name;
}

// CREATE Employee
function addUser() {
    let token = sessionStorage.getItem("token");
    const addPassword = document.getElementById('add-password').value.trim();
    const addName = document.getElementById('add-user-name').value.trim();
    const user = {
        UserId: 0,
        Name: addName,
        Password: addPassword,
        agentId: "Agent"
    }

    fetch(url, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': "Bearer " + token
        },
        body: JSON.stringify(user)
    })
        .then(response => response.json())
        .then(() => {
            GetAllEmployees();
        })
        .catch(() => alert("Not Authorized!!"));
}

// DELATE Employee
function deleteUser(id) {
    let token = sessionStorage.getItem("token");
    fetch(`${url}/${id}`, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
        },
    }).then(() => GetAllEmployees())
        .catch(error => console.log('Unable to delete user.', error));
}

function displayEditFormEmployee(empId) {
    const employee = Employee.find(e => e.userId == empId);
    if (employee) {
        document.getElementById('edit-name').value = employee.name;
        document.getElementById('edit-password').value = employee.password;
        document.getElementById('editFormEmployee').style.display = 'block';
        updateEmployee(employee);
    } else {
        console.error('Employee not found');
    }
    GetAllEmployees();
}

// UPDATE Employee
function updateEmployee(employee) {
    let token = sessionStorage.getItem("token");

    document.getElementById('saveEdit').onclick = () => {
        const updatedEmployee = {
            UserId: employee.userId,
            Name: document.getElementById('edit-name').value,
            Password: document.getElementById('edit-password').value,
            TaskManager: false
        };

        fetch(`${url}/${employee.userId}`, {
            method: 'PUT',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': "Bearer " + token
            },
            body: JSON.stringify(updatedEmployee)
        })
            .then(() => getEmployeeById())
            .catch(error => console.error('Unable to update item.', error));
    }
    GetAllEmployees();
    //closeInput();
}

// התנתקות וחזרה לדף הלוגין
function logOut() {
    location.href = "/index.html";
}

