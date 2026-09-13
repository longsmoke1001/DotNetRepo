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
      setError('Failed to update the note. Please try again later.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{ marginTop: 10, padding: 15, border: '1px solid #007bff', borderRadius: 8 }}>
      <h3>Edit Note</h3>
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
        <button type="submit" disabled={loading} style={{ padding: '8px 16px', marginRight: 10 }}>
          {loading ? 'Updating...' : 'Update'}
        </button>
        <button type="button" onClick={onCancel} style={{ padding: '8px 16px' }}>
          Cancel
        </button>
        {error && <p style={{ color: 'red', marginTop: 10 }}>{error}</p>}
      </form>
    </div>
  );
}

export default EditNote;