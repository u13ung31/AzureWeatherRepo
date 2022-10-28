const GetFavoritePlace = async (UserID) => {
    
    return new Promise(function(resolve, reject) {
        const Http = new XMLHttpRequest();
        const url='http://localhost:7071/api/HttpTriggerGetList?UserID='+UserID;
        Http.open("GET", url);
        Http.setRequestHeader('Access-Control-Allow-Origin', '*');
        Http.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
        Http.onreadystatechange = (e) => {
        
            const obj = JSON.parse(Http.responseText);

            console.log(obj);
            resolve(obj)

        }
    })
};

export {GetFavoritePlace};