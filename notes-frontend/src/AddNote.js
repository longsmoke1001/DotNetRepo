import { useState } from 'react';
import axios from 'axios';

function AddNote({ token, onNoteAdded }) {
  const [title, setTitle] = useState('');
  const [content, setContent] = useState('');
  const [category, setCategory] = useState('general');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      await axios.post(
        'http://localhost:5027/api/Notes',
        {
          title,
          content,
          category
        },
        {
          headers: {
            Authorization: `Bearer ${token}`
          }
        }
      );

      // 清空表單
      setTitle('');
      setContent('');
      setCategory('general');

      // 通知父元件重新 fetch Notes
      onNoteAdded();

    } catch (err) {
      console.error('Add note error:', err);
      setError('新增失敗，請稍後再試');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{ marginBottom: 20, padding: 15, border: '1px solid #ccc', borderRadius: 8 }}>
      <h3>新增筆記</h3>
      <form onSubmit={handleSubmit}>
        <div>
          <label>標題：</label>
          <input
            type="text"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            required
            style={{ width: '100%', padding: 8, marginBottom: 10 }}
          />
        </div>
        <div>
          <label>內容：</label>
          <textarea
            value={content}
            onChange={(e) => setContent(e.target.value)}
            required
            rows={3}
            style={{ width: '100%', padding: 8, marginBottom: 10 }}
          />
        </div>
        <div>
          <label>分類：</label>
          <select
            value={category}
            onChange={(e) => setCategory(e.target.value)}
            style={{ width: '100%', padding: 8, marginBottom: 10 }}
          >
            <option value="general">一般</option>
            <option value="dairy">日記</option>
            <option value="password">密碼</option>
          </select>
        </div>
        <button type="submit" disabled={loading} style={{ padding: '10px 20px' }}>
          {loading ? '新增中...' : '新增'}
        </button>
        {error && <p style={{ color: 'red', marginTop: 10 }}>{error}</p>}
      </form>
    </div>
  );
}

export default AddNote;