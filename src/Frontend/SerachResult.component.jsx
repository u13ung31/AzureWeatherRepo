import React,{useState,useEffect} from 'react';
import { useLocation,Link } from 'react-router-dom'
import './SearchBox.Styling.css'
import { SearchResultJS } from '../Backend/Services/SearchResultQueryFinder';
const SearchResult = () =>{
    const[DataisLoaded,SetDataLoaded] = useState(false);
    const[items,SetItems] = useState([]);

    useEffect(() => {
        let isMounted = true;               // note mutable flag
        if(!DataisLoaded){
            SearchResultJS(from).then(json =>{
                if (isMounted){
                    console.log(json);
                    SetItems(json)
                    SetDataLoaded(true)

                }
            })
        return () => { isMounted = false };
        }
        
    });
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
    
    if(checkIfSearchWork && DataisLoaded){
        const names = []
        let countDown = 0;
        items.forEach(data =>{
            names.push(
                <form className='FormSearch' key={countDown}>
                    <div>
                        <div>
                            <h4>{data.name}</h4>
                        </div>
                        
                        <div>
                            <p>{data.country}</p>
                        </div>
                    </div>
                    <div className='DivLink'>
                            <button><i className="fa fa-star"></i></button>
                    </div>
                   

                </form>
            )
            countDown++;
        });

        return (
            <div>
            {names}
            </div>)          
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