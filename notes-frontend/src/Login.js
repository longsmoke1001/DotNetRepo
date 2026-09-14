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
      console.log('API URL:', process.env.REACT_APP_API_URL);
      const response = await axios.post(`${process.env.REACT_APP_API_URL}/api/Auth/login`, {
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
    <div className="max-w-md mx-auto mt-20 bg-white p-8 rounded-lg shadow-md">
      <h2 className="text-2xl font-bold text-center mb-6">登入</h2>
      <form onSubmit={handleSubmit}>
        <div className="mb-4">
          <label className="block mb-2 font-medium">電郵：</label>
          <input
            type="text"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
            className="w-full px-3 py-2 border border-gray-300 rounded focus:outline-none focus:border-blue-500"
          />
        </div>
        <div className="mb-4">
          <label className="block mb-2 font-medium">密碼：</label>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
            className="w-full px-3 py-2 border border-gray-300 rounded focus:outline-none focus:border-blue-500"
          />
        </div>
        <button
          type="submit"
          disabled={loading}
          className="w-full bg-blue-500 hover:bg-blue-600 text-white py-2 rounded transition disabled:opacity-50"
        >
          {loading ? '登入中...' : '登入'}
        </button>
        {error && <p className="text-red-500 mt-3 text-sm">{error}</p>}
      </form>
    </div>
  );
}

export default Login;