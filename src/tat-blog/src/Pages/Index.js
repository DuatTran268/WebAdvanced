  import React from 'react';
import { useLocation } from 'react-router-dom';
import PostSearch from '../Components/posts/PostSearch';


function Index() {
  const querySearch = new URLSearchParams(useLocation().search);
  const keyword = querySearch.get('Keyword') ?? '';

  return (
    <div className='container'>
      <h1 className='mt-3'>
        {keyword.length !== 0 ? (
          <>
            Bài viết theo từ khóa:
            <span className='text-danger'> {keyword}</span>
          </>
        ) : (
          'Danh sách bài viết'
        )}
      </h1>

      <PostSearch querySearch={querySearch} />
    </div>
  );
}

export default Index;
