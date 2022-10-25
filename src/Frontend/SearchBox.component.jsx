import React from 'react'
import './SearchBox.Styling.css'
import {  Link } from 'react-router-dom'

const SearchBox = () =>{

  return (
    <form className='topnav'>
    <input type="text" placeholder="Search.." name="search"/>
    <Link to={'/sign-up'}>
        <button><i className="fa fa-search"></i></button>
    </Link>
  </form>)

}

export default SearchBox;