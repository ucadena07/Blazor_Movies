function my_function(message) {
  console.log("from utilities " + message)
}

var arrow_function = (message) => {
  console.log("from utilities " + message)
}


function dotNetInstanceInovation(donetHelper) {
  donetHelper.invokeMethodAsync("IncrementCount")
}

function initializeInactivityTimer(dotnetHelper) {
    var timer
    document.onmousemove = resetTimer;
    document.onkeypress = resetTimer;

    function resetTimer() {
        clearTimeout(timer)
        timer = setTimeout(logout, 3000)
    }

    function logout() {
        dotnetHelper.invokeMethodAsync("Logout")
    }

}

function setLocalStorage(key, value) {
    localStorage[key] = value
}

function getFromLocalStorage(key) {
    return localStorage[key]
}