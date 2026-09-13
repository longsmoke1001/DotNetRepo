import { useState } from 'react';
import axios from 'axios';

function EditNote({ token, note, onNoteUpdated, onCancel }) {
  const [title, setTitle] = useState(note.title);
  const [content, setContent] = useState(note.content);
  const [category, setCategory] = useState(note.category);
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      await axios.put(
        `http://localhost:5027/api/Notes/${note.id}`,
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

      onNoteUpdated();

    } catch (err) {
      console.error('Update note error:', err);
      setError('更新失敗，請稍後再試');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{ marginTop: 10, padding: 15, border: '1px solid #007bff', borderRadius: 8 }}>
      <h3>編輯筆記</h3>
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
        <button type="submit" disabled={loading} style={{ padding: '8px 16px', marginRight: 10 }}>
          {loading ? '更新中...' : '更新'}
        </button>
        <button type="button" onClick={onCancel} style={{ padding: '8px 16px' }}>
          取消
        </button>
        {error && <p style={{ color: 'red', marginTop: 10 }}>{error}</p>}
      </form>
    </div>
  );
}

export default EditNote;