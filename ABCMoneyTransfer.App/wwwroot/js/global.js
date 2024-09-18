

Vue.prototype.$showSuccessMessage = function (message) {
    if (message) {
        window.localStorage.setItem('message', message);
    }
}

Vue.prototype.$showErrorMessage = function (message) {
    if (message) {
        window.localStorage.setItem('error-message', message);
    }
}

Vue.prototype.$redirect = function (url) {
    window.location.href = url;
}

Vue.prototype.$reload = function (url) {
    window.location.reload();
}

Vue.prototype.$formatDate = function (date, format) {
    return moment(date).format(format)
}


// ERROR SERIALIZATION
Vue.prototype.$handleError = function (error) {
    const serialized = serializeError(error.response);
    const serialized1 = serializeError(error);


    (serialized);
    console.log(serialized1);

    if (Object.prototype.hasOwnProperty.call(serialized, 'data')) {
        Quasar.Notify.create({
            message: serialized.data,
            color: 'negative',
            icon: 'warning'
        });
    } else if (Object.prototype.hasOwnProperty.call(serialized1, 'message')) {
        Quasar.Notify.create({
            message: serialized.message,
            color: 'negative',
            icon: 'warning'
        });
    }
}

// Loading Related
Vue.prototype.$showLoader = function () {
    Quasar.Loading.show({ message: "Please Wait... <br /> Data is Processing in Server."});
}

Vue.prototype.$hideLoader = function () {
    Quasar.Loading.hide();
}