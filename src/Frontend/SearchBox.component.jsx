import React from 'react'
import './SearchBox.Styling.css'
import {  Link } from 'react-router-dom'
import {useState} from 'react';
import { useNavigate } from 'react-router-dom';
import { NavLink } from 'react-router-dom';

const SearchBox = () =>{
  const [searchQuery, setSearchQuery] = useState('');
  const navigate = useNavigate();

  const handleChange = event => {
    setSearchQuery(event.target.value);

    console.log('value is:', event.target.value);
  };

  return (
    <form className='topnav'>
    <input type="text" 
    placeholder="Search.." 
    value={searchQuery}  
    name="search"
    id="search"
    onChange={handleChange}
    />
    <NavLink to="/Search-Result" state={{ from: searchQuery }}>
        <button><i className="fa fa-search"></i></button>
    </NavLink>
  </form>)

}

export default SearchBox;