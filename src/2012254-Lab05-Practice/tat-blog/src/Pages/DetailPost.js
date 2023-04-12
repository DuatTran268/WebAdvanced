import React from "react";
import { useParams } from "react-router-dom";
import PostSearch from "../Components/PostSearch";

const DetailPost = () => {


  const params = useParams();

  return (
    <>
      <div className="card-body mb-5">
      <div className='single-post mb-5'>
                <h1 className='mb-3'>
                   Tên
                </h1>

                <div className='h-25 overflow-hidden mb-4'>
                    <img
                        className='featured-image'
                        src={'/image/1'}
                        alt={'post.title'} />
                </div>

                <div className='mb-3 d-flex justify-content-between'>
                    <div>
                        Published on:
                        <span className='fw-bold text-primary'>
                            {/* {post.postedDate} */}
                            07/04/2023
                        </span>
                    </div>
                    <div>
                        By :
                        <a href="/"
                            // className='text-decoration-none fw-bold'
                            // href={`/author/${post.author.urlSlug}`}
                            >
                            {/* {post.author.fullName} */}
                          Duat
                        </a>
                    </div>
                    <div>
                        Category:
                        <a
                            className='text-decoration-none fw-bold'
                            // href={`/categories/${post.category.urlSlug}`}
                            href="/"
                            >
                            {/* {post.category.name} */} Name
                        </a>
                    </div>
                </div>

                <div className='mb-3'>
                    {/* <TagList tags={post.tags} /> */} Tag
                </div>

                <p className='fw-bold text-success'>
                    {/* {post.shortDescription} */}
                lorem is
                </p>

                <div
                    className='post-content' 
                    // dangerouslySetInnerHTML={{ __html: post.description }}
                    >
                    lorem
                </div>
            </div>

      <PostSearch postQuery={{postSlug:params.slug}}/>
      </div>
    </>
  )
}

export default DetailPost;