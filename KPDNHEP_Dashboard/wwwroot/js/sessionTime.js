var timeOut = localStorage.getItem("sessiontime");
let timer, currSeconds = 0;
var IDLE_TIMEOUT = 10;
function resetTimer() {
    clearInterval(timer);
    currSeconds = 0;
    timer =
        setInterval(startIdleTimer, 1000);
}

window.onload = resetTimer;
window.onmousemove = resetTimer;
window.onmousedown = resetTimer;
window.ontouchstart = resetTimer;
window.onclick = resetTimer;
window.onkeypress = resetTimer;

function startIdleTimer() {
    currSeconds++;

    if (currSeconds >= timeOut) {
        window.clearInterval(currSeconds);
        $.ajax({
            url: '/Account/SessionOut',
            type: "GET",
            success: function () {
                window.location.href = "/";
            }
        });
    }
}
