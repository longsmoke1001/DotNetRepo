import { useState } from 'react';
import axios from 'axios';

function Login({ onLoginSuccess }) {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    // 🔍 DEBUG
    console.log('傳送嘅 username:', email);
    console.log('傳送嘅 password:', password);
    try {
      // ⚠️ 將 5001 改返你做 API 嘅 port
      const response = await axios.post('http://localhost:5027/api/Auth/login', {
        username: email,
        password
      });

      const token = response.data.token;
      localStorage.setItem('token', token);
      onLoginSuccess(token);

    } catch (err) {
      setError('Login failed. Please check your email and password.');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{ maxWidth: 400, margin: '50px auto', padding: 20, border: '1px solid #ccc', borderRadius: 8 }}>
      <h2>Log In</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Email:</label>
          <input
            type="text"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
            style={{ width: '100%', padding: 8, marginBottom: 10 }}
          />
        </div>
        <div>
          <label>Password:</label>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
            style={{ width: '100%', padding: 8, marginBottom: 10 }}
          />
        </div>
        <button type="submit" disabled={loading} style={{ padding: '10px 20px' }}>
          {loading ? 'Logging in...' : 'Log In'}
        </button>
        {error && <p style={{ color: 'red', marginTop: 10 }}>{error}</p>}
      </form>
    </div>
  );
}

export default Login;