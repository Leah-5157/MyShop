
const GetDataFromDocumentForRegister = () => {
    const UserName = document.querySelector("#userName1").value;
    const Password = document.querySelector("#password1").value;
    const FirstName = document.querySelector("#firstName").value;
    const LastName = document.querySelector("#lastName").value;
    return ({ UserName, Password, FirstName, LastName });
}
const Register = async() => {
    const newUser = GetDataFromDocumentForRegister();
    try {
        
        const response = await fetch("../api/Users", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(newUser)
        });
        console.log(response)
        if (!response.ok) { 
            console.log(`HTTP error! status:${newUser.status}`)
            alert("שם משתמש או סיסמה אינם תקינים") }
      else
           alert("New user!")
    } catch (error) {
        console.log(error)
    }
}
const GetDataFromDocumentForLogin = () => {
    const UserName = document.querySelector("#userName").value;
    const Password = document.querySelector("#password").value;
    return ({UserName, Password })
  
}
const Login = async () => {
    const existUser = GetDataFromDocumentForLogin();
    try {
        const loginPost = await fetch(`../api/Users/Login?UserName=${existUser.UserName}&Password=${existUser.Password}`, {
            method: "POST",
            headers: {
                'Content-type': 'application/json'
            },
            query: {
                UserName: existUser.UserName,
                Password: existUser.Password
            }
        });
        const data = await loginPost.json();
        console.log(data)
        if (!loginPost.ok) {
            throw new Error(`HTTP error! status:${loginPost.status}`);
        }
        else
          alert("conected!!")
        sessionStorage.setItem("id", data.id)
        window.location.href = 'Update.html'

    } catch (error) {
        alert("try again")
        console.log(error)
    }

}

const Update = async () => {
    const newDetails = GetDataFromDocumentForRegister();
    try {

        const responsePut = await fetch(`../api/Users/${sessionStorage.getItem("id")}`, {
            method: "PUT",
            headers: {
                'Content-type': 'application/json'
            },
            body: JSON.stringify(newDetails)
           

        });
        const dataPut = await responsePut.json
        if (!responsePut.ok)
            alert("try again")
        else {
            alert("פרטים עודכנו בהצלחה!")
        }
    }
    
    catch (error) {
        alert("try again")
        console.log(error)
    }
}
const visible = () => {
    const newUser = document.querySelector(".newUser")
    newUser.classList.remove("newUser")
}
const Password = async() => {
    const newUser = GetDataFromDocumentForRegister();
    try {
        const response = await fetch("../api/Users/Password", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(newUser.Password)
        });
        const Response = await response.json();
        const progress = document.querySelector("#progress")
        console.log(Response)
        progress.value = Response;
    } catch (error) {
        console.log(error)
    }
}


