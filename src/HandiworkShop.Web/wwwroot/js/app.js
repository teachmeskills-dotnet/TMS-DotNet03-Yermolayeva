var avatarInput = document.getElementById("NewAvatar");

var loadFile = function (event) {
    var image = document.getElementById('output');
    image.src = URL.createObjectURL(event.target.files[0]);
};

function run() {
    avatarInput.addEventListener("input", loadFile);
}

document.addEventListener("DOMContentLoaded", run);