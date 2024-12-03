document.querySelector('.toggle-password').addEventListener('click', function () {
    const passwordField = document.getElementById('password');
    const passwordType = passwordField.getAttribute('type');
    if (passwordType === 'password') {
        passwordField.setAttribute('type', 'text');
        this.classList.remove('bi-eye-slash');
        this.classList.add('bi-eye');
    } else {
        passwordField.setAttribute('type', 'password');
        this.classList.remove('bi-eye');
        this.classList.add('bi-eye-slash');
    }
});
