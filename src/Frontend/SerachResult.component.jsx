import React,{useState,useEffect} from 'react';
import { useLocation } from 'react-router-dom'
import { SearchResultJS } from '../Backend/SearchResultQueryFinder';
import ImportantString from "../Enum/ImportantStrings.json";
const SearchResult = () =>{
    const[DataisLoaded,SetDataLoaded] = useState(false);
    const[items,SetItems] = useState([]);

    useEffect(() => {
        let isMounted = true;               // note mutable flag

        /*SearchResultJS(from).then(json =>{
                if (isMounted){
                    console.log(json);
                    SetItems(json)
                    SetDataLoaded(true)

                }
            })*/
        return () => { isMounted = false };
    });
    console.log(ImportantString);
    let checkIfSearchWork = true;
    var from = ""; 
    const location = useLocation()
    if (location.state === undefined || location.state === null || location.state.from.length === 0) {

        from = ""
        checkIfSearchWork = false

    }
    else{

        from = location.state.from

    }


    var matches = from.match(/\d+/g);
    if (matches != null) {
        checkIfSearchWork = false;
    }
    
    if(checkIfSearchWork){
        return (
            <form>
                <div>
                {from}
                </div>
            </form>)
        /*
        const names = []
        const studentDetails = ['Alex', 'Anik', 'Deven', 'Rathore'];
        let countdown = 0  
        SearchResultJS(from).then(json =>{
            json.forEach((data) => {
                names.push(<h3 key={countdown}>{data.name}</h3>)
                countdown++;
            })
            return (
                <div className='container'>
                  {names}
                </div>
              )
        })
      
        
        /*SearchResultJS(from).then(data =>{
            data.forEach((data) => {
                names.push(<h3>{data.name}</h3>)
            })
        })
       
        return (
            <form>
                <div>
                {names}
                </div>
            </form>)*/
           
            
    }
    if (DataisLoaded){

        items.forEach(data =>{

        });

        return ( <form>
            <div>
                <h1>No search result found</h1>
            </div>
        </form>) ;
    }
    else{
        return ( <form>
            <div>
                <h1>No search result found</h1>
            </div>
        </form>) ;
    }

}

export default SearchResult;