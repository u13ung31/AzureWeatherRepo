
const Login = async (json) => {

    const Http = new XMLHttpRequest();
    const url='http://localhost:7071/api/HttpTriggerLogin?Username='+json.UserName+'&Password='+json.Password;
    Http.open("GET", url);
    Http.setRequestHeader('Access-Control-Allow-Origin', '*');
    Http.setRequestHeader("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
    Http.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    Http.send(JSON.stringify(json));    
    Http.onreadystatechange = (e) => {
      console.log(Http.responseText)
    }
    
};
export {Login};