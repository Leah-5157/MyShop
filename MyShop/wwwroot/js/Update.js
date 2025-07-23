// Update.js - Update User (English)

function checkPasswordStrength() {
    const password = document.getElementById('newPassword').value;
    const progress = document.getElementById('progress');
    const label = document.getElementById('strengthLabel');
    if (!password) {
        progress.value = 0;
        label.textContent = '';
        return;
    }
    fetch('../api/Users/Password', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(password)
    })
    .then(res => res.json())
    .then(score => {
        progress.value = score;
        let txt = '';
        switch (score) {
            case 0: txt = 'Very Weak'; break;
            case 1: txt = 'Weak'; break;
            case 2: txt = 'Medium'; break;
            case 3: txt = 'Strong'; break;
            case 4: txt = 'Very Strong'; break;
        }
        label.textContent = txt;
    });
}

function updateUser() {
    const id = sessionStorage.getItem('id');
    if (!id) {
        // Not logged in, redirect to login and return to update after login
        sessionStorage.setItem('redirectAfterLogin', 'Update.html');
        window.location.href = 'Login.html';
        return;
    }
    const userName = document.getElementById('userName').value;
    const currentPassword = document.getElementById('currentPassword').value;
    const newPassword = document.getElementById('newPassword').value;
    const firstName = document.getElementById('firstName').value;
    const lastName = document.getElementById('lastName').value;
    if (!currentPassword || !newPassword || !firstName) {
        alert('Please fill all required fields');
        return;
    }
    fetch(`../api/Users/${id}/UpdatePassword`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            UserName: userName,
            CurrentPassword: currentPassword,
            NewPassword: newPassword,
            FirstName: firstName,
            LastName: lastName
        })
    })
    .then(async res => {
        if (!res.ok) {
            const err = await res.text();
            alert(err || 'Update failed');
        } else {
            alert('User details updated successfully!');
            window.location.href = 'ShoppingBag.html';
        }
    })
    .catch(() => alert('Update failed'));
}

document.addEventListener('DOMContentLoaded', () => {
    // Redirect back to Update.html after login if needed
    if (sessionStorage.getItem('redirectAfterLogin') === 'Update.html') {
        sessionStorage.removeItem('redirectAfterLogin');
        // Already on Update.html, do nothing
    }
    // Optionally fetch username from sessionStorage or server
    const userName = sessionStorage.getItem('userName');
    if (userName) {
        document.getElementById('userName').value = userName;
    }
});
