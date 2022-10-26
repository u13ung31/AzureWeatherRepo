const AddFavoritePlace = async (json) => {
    
    
    const Http = new XMLHttpRequest();
    const url='http://localhost:7071/api/HttpTriggerAddToList';
    Http.open("POST", url);
    Http.setRequestHeader('Access-Control-Allow-Origin', '*');
    Http.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    Http.send(JSON.stringify(json));    
    Http.onreadystatechange = (e) => {
      console.log(Http.responseText)
    }

};
export {AddFavoritePlace};