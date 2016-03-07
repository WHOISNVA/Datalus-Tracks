if (!sabio.services.track) {
    sabio.services.track = {};
}
//mydata IS REFERRRING TO THE OBJECT THAT IS BEING RECIEVED BY ANY CALL IN THE SERVICE

sabio.services.track.Create = function (myData, onSuccess, onError) {

    var url = "/api/track";
    var settings = {
        cache: false
		, contentType: "application/json; charset=UTF-8"
		, data: JSON.stringify(myData)
		, dataType: "json"
		, success: onSuccess
		, error: onError
		, type: "POST"
    };
    $.ajax(url, settings);
}

sabio.services.track.Update = function (myData, id, onSuccess, onError) {

    var url = "/api/track/" + id;
    var settings = {
        cache: false
		, contentType: "application/json; charset=UTF-8"
		, data: JSON.stringify(myData)
		, dataType: "json"
		, success: onSuccess
		, error: onError
		, type: "PUT"
    };
    $.ajax(url, settings);
}

sabio.services.track.GetById = function (id, onSuccess, onError) {
    var url = "/api/track/" + id;
    var settings = {
        cache: false
		, contentType: "application/x-www-form-urlencoded; charset=UTF-8"
		, dataType: "json"
		, success: onSuccess
		, error: onError
		, type: "GET"
    };
    $.ajax(url, settings);
}

sabio.services.track.getAll = function (onSuccess, onError) {
    var url = "/api/track/";
    var settings = {
        cache: false
		, contentType: "application/x-www-form-urlencoded; charset=UTF-8"
		, dataType: "json"
		, success: onSuccess
        , error: onError
		, type: "GET"
    };
    $.ajax(url, settings);
}

sabio.services.track.delete = function (id, onSuccess, onError) {
    var url = "/api/track/" + id;
    var settings = {
        cache: false
        , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
        , dataType: "json"
        , success: onSuccess
        , error: onError
        , type: "DELETE"
    };
    $.ajax(url, settings);
}