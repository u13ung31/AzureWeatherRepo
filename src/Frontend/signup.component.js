import { useState } from 'react';
import SignUpUser from '../Backend/SignUpUser';

const SignUp = () => {
  
  const [form, setForm] = useState({})
  const [errors, setErrors] = useState({})
  const [message, setMessage] = useState("");

  const setField = (field, value) => {
    setForm({
        ...form, [field]: value
    })
    // Check and see if errors exist, and remove them from the error object:
    if (!!errors[field]) setErrors({
        ...errors,
        [field]: null
    })
  }

  const findFormErrors = () => {
    const { username, password } = form
    const newErrors = {}

    if (!username || username === '') newErrors.username = 'Du måste ange en email'
    else if (username.length > 25) newErrors.email = 'Email får max vara 50 tecken'

    if (!password || password === '') newErrors.password = 'Du måste skriva en lösenord'
    else if (password.length > 255) newErrors.password = 'Lösenord får max 500 tecken!'

    return newErrors
  }

  const handleSubmit = e => {
    e.preventDefault()
    // get our new errors
    const newErrors = findFormErrors()
    // Conditional logic:
    if (Object.keys(newErrors).length > 0) {
        // We got errors!
    }
    else {
        // No errors! Put any logic here for the form submission!

        let jsonBody = {
            "UserName": form.username,
            "Password": form.password
        }

        console.log(jsonBody);
        //SignUpUser(jsonBody);
        // navigation
        // method i Backend
    }
  }
    return (
      <form onSubmit={handleSubmit}>
        <h3>Sign Up</h3>
        <div className="mb-3">
          <label>Username</label>
          <input
            type="text"
            className="form-control"
            placeholder="Enter username"
            onChange={e => setField('username', e.target.value)}
          />
        </div>
        <div className="mb-3">
          <label>Password</label>
          <input
            type="password"
            className="form-control"
            placeholder="Enter password"
            onChange={e => setField('password', e.target.value)}
          />
        </div>
        <div className="d-grid">
          <button type="submit" className="btn btn-primary">
            Sign Up
          </button>
        </div>
        <p className="forgot-password text-right">
          Already registered <a href="/sign-in">sign in?</a>
        </p>
      </form>
    )
}
export default SignUp;