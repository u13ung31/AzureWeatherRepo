import React,{useState,useEffect} from 'react';
import { useLocation,Link } from 'react-router-dom'
import './SearchBox.Styling.css'
import { GetFavoritePlace } from '../Backend/DataServices/GetFavoriteWeather';
import { DeletePlace } from '../Backend/DataServices/DeletePlace';

const SearchResult = () =>{
    const[DataisLoaded,SetDataLoaded] = useState(false);
    const[items,SetItems] = useState([]);

    useEffect(() => {
        let isMounted = true;
        if(!DataisLoaded){

            let user = {UserID:1}

            GetFavoritePlace(user).then(json =>{
                if (isMounted){
                    console.log(json);
                    SetDataLoaded(true);
                    SetItems(json)
                }
            })
        return () => { isMounted = false };
        }
        
    });
    
    const DeleteButton = (ID) =>{

        DeletePlace(ID)
    }
   
    if(DataisLoaded){
        const places = []
        let countDown = 0;
        items.forEach(data =>{
            places.push(
                <form className='FormSearch' key={countDown}>
                    <div>
                        <div>
                            <h4>{data.CityName}</h4>
                        </div>
                        
                        <div>
                            <p></p>
                        </div>
                    </div>
                    <div className='DivLink'>
                            <button type="button" onClick={() => {
                            DeleteButton(data.FavoriteWeatherId);
                            }}>                        
                                <i className="fa fa-trash"></i>
                            </button>
                    </div>
                </form>
            )
            countDown++;
        });

        return (
            <div>
            {places}
            </div>)          
    }
    else{
        return ( <form>
            <div>
                <h1>Data is being fetch</h1>
            </div>
        </form>) ;
    }

}

export default SearchResult;