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
      setError('登入失敗，請檢查電郵同密碼');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{ maxWidth: 400, margin: '50px auto', padding: 20, border: '1px solid #ccc', borderRadius: 8 }}>
      <h2>登入</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>電郵：</label>
          <input
            type="text"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
            style={{ width: '100%', padding: 8, marginBottom: 10 }}
          />
        </div>
        <div>
          <label>密碼：</label>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
            style={{ width: '100%', padding: 8, marginBottom: 10 }}
          />
        </div>
        <button type="submit" disabled={loading} style={{ padding: '10px 20px' }}>
          {loading ? '登入中...' : '登入'}
        </button>
        {error && <p style={{ color: 'red', marginTop: 10 }}>{error}</p>}
      </form>
    </div>
  );
}

export default Login;