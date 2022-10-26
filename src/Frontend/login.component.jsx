import { useState } from 'react';
import {Login as LoginUser} from '../Backend/Login';

const Login = () => {

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
    const { email, password } = form
    const newErrors = {}

    if (!email || email === '') newErrors.email = 'Du måste ange en email'
    else if (email.length > 25) newErrors.email = 'Email får max vara 50 tecken'

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
            "UserName": form.email,
            "Password": form.password
        }

        console.log(jsonBody);
        LoginUser(jsonBody);
        // method i Backend
    }
  }
  
  return (
      <form onSubmit={handleSubmit}>
        <h3>Sign In</h3>
        <div className="mb-3">
          <label>Email address</label>
          <input
            type="text"
            className="form-control"
            placeholder="Enter email"
            onChange={e => setField('email', e.target.value)}
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
        <div className="mb-3">
          <div className="custom-control custom-checkbox">
            <input
              type="checkbox"
              className="custom-control-input"
              id="customCheck1"
            />
            <label className="custom-control-label" htmlFor="customCheck1">
              Remember me
            </label>
          </div>
        </div>
        <div className="d-grid">
          <input type="submit" value="Submit"className="btn btn-primary" />
        </div>
      </form>
    )
}

export default Login;