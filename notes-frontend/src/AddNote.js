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
      setError('Failed to add the note. Please try again later.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{ marginBottom: 20, padding: 15, border: '1px solid #ccc', borderRadius: 8 }}>
      <h3>Add Note</h3>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Title:</label>
          <input
            type="text"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            required
            style={{ width: '100%', padding: 8, marginBottom: 10 }}
          />
        </div>
        <div>
          <label>Content:</label>
          <textarea
            value={content}
            onChange={(e) => setContent(e.target.value)}
            required
            rows={3}
            style={{ width: '100%', padding: 8, marginBottom: 10 }}
          />
        </div>
        <div>
          <label>Category:</label>
          <select
            value={category}
            onChange={(e) => setCategory(e.target.value)}
            style={{ width: '100%', padding: 8, marginBottom: 10 }}
          >
            <option value="general">General</option>
            <option value="dairy">Diary</option>
            <option value="password">Password</option>
          </select>
        </div>
        <button type="submit" disabled={loading} style={{ padding: '10px 20px' }}>
          {loading ? 'Adding...' : 'Add Note'}
        </button>
        {error && <p style={{ color: 'red', marginTop: 10 }}>{error}</p>}
      </form>
    </div>
  );
}

export default AddNote;