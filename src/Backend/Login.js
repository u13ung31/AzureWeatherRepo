
const Login = async (json) => {
  return new Promise(function(resolve, reject) {
    const Http = new XMLHttpRequest();
    const url='http://localhost:7071/api/HtttpTriggerLogin?UserName='+json.UserName+'&Password='+json.Password;
    console.log(url)
    Http.open("GET", url);
    Http.setRequestHeader('Access-Control-Allow-Origin', '*');
    Http.setRequestHeader("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
    Http.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    Http.send(JSON.stringify(json));    
    Http.onreadystatechange = (e) => {
      try {
        console.log(Http.responseText);
        if(Http.responseText.length > 5){
        const obj = JSON.parse(Http.responseText);
        resolve(obj);
        }
        

      } catch (error) {
        reject(error);
      }

    }
  })    
}
export {Login};