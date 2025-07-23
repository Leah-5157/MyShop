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
            alert("בדוק את תקינות השדות!")
        }
        else {
            const data = await response.json();
            alert("New user!");
            sessionStorage.setItem("id", data.id.toString());
            sessionStorage.setItem("userName", data.userName);
            window.location.href = 'ShoppingBag.html';
        }
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
        const loginPost = await fetch("../api/Users/Login", {
            method: "POST",
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(existUser)
        });
        if (!loginPost.ok) {
            const errorText = await loginPost.text();
            alert(errorText || "Login failed");
            return;
        }
        const data = await loginPost.json();
        console.log(data);
        alert("Connected!");
        sessionStorage.setItem("id", data.id);
        sessionStorage.setItem("userName", data.userName);
        // Check if redirectAfterLogin is set
        const redirect = sessionStorage.getItem('redirectAfterLogin');
        if (redirect) {
            sessionStorage.removeItem('redirectAfterLogin');
            window.location.href = redirect;
        } else {
            window.location.href = 'ShoppingBag.html';
        }
    } catch (error) {
        alert("try again");
        console.log(error);
    }
}

const visible = () => {
    const newUser = document.querySelector(".newUser")
    newUser.classList.remove("newUser")
}

const Password = async() => {
    const newUser = GetDataFromDocumentForRegister();
    try {
        const response = await fetch("../api/Users/password", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(newUser.Password)
        });
        console.log(response)
        const data = await response.json();
        const progress = document.querySelector("#progress");
             progress.value = data;
      
    } catch (error) {
        console.log(error)
    }
}