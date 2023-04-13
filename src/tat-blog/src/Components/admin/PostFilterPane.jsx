import { useState, useEffect } from "react";
import { Button, Form } from "react-bootstrap";
import { Link } from "react-router-dom";
import { getFilter } from "../../Services/BlogRepository";

import { useSelector, useDispatch } from "react-redux";
import {
  reset, 
  updateAuthorId,
  updateCategoryId,
  updateKeyword,
  updateMonth,
  updateYear
} from '../../Redux/Reducer';

const PostFilerPane = () => {
  const postFilter = useSelector(state => state.postFilter), dispatch = useDispatch(),
  [filter, setFilter] = useState({
    authorList : [],
    categoryList: [],
    monthList: []
  });

  const handleReset = (e) => {
    dispatch(reset());
  };


  //const current = new Date(),
    // [keyword, setKeyword] = useState(""),
    // [authorId, setAuthorId] = useState(""),
    // [categoryId, setCategoryId] = useState(""),
    // [year, setYear] = useState(current.getFullYear()),
    // [month, setMonth] = useState(current.setFullYear()),
    // [postFilter, setPostFilter] = useState({
    //   authorList: [],
    //   categoryList: [],
    //   monthList: [],
    // });

  const handleSubmit = (e) => {
    e.preventDefault();
  };

  useEffect(() => {
    getFilter().then((data) => {
      if (data) {
        setFilter({
          authorList: data.authorList,
          categoryList: data.categoryList,
          monthList: data.monthList,
        });
      } else {
        setFilter({
          authorList: [],
          categoryList: [],
          monthList: [],
        });
      }
    });
  }, []);
  

  return (
    <Form
      method="get"
      onReset={handleReset}
      onSubmit={handleSubmit}
      className="row gy-2 gx-3 align-items-center p-2"
    >
      <Form.Group className="col-auto">
        <Form.Label className="visually-hidden">Keyword</Form.Label>
        <Form.Control
          type="text"
          placeholder="Nhập vào từ khoá ..."
          name='keyword'
          value={postFilter.keyword}
          onChange={(e) => dispatch(updateKeyword(e.target.value))}
        />
      </Form.Group>

      <Form.Group className="col-auto">
        <Form.Label className="visually-hidden">AuthorId</Form.Label>
        <Form.Select
          name="authorId" 
          value={postFilter.authorId}
          onChange={(e) => dispatch(updateAuthorId(e.target.value))}
          title=" Author Id"
        >
          <option value="">-- Chọn tác giả --</option>
          {filter.authorList.length > 0 &&
            filter.authorList.map((item, index) => (
              <option key={index} value={item.value}>
                {item.text}
              </option>
            ))}
        </Form.Select>
      </Form.Group>

      <Form.Group className="col-auto">
        <Form.Label className="visually-hidden">CategoryId</Form.Label>

        <Form.Select
          name="categoryId"
          value={postFilter.categoryId}
          onChange={(e) => dispatch(updateCategoryId(e.target.value))}
          title="Category Id"
        >
          <option value="">-- Chọn chủ đề --</option>
          {filter.categoryList.length > 0 &&
            filter.categoryList.map((item, index) => (
              <option key={index} value={item.value}>
                {item.text}
              </option>
            ))}
        </Form.Select>
      </Form.Group>

      <Form.Group className="col-auto">
        <Form.Label className="visually-hidden">Year</Form.Label>

        <Form.Control type="number" 
          placeholder="Nhập năm ..."
          name="year"
          value={postFilter.year}
          max={postFilter.year}
          onChange={e => dispatch(updateYear(e.target.value))}
        />
      </Form.Group>

      <Form.Group className="col-auto">
        <Form.Label className="visually-hidden">Month</Form.Label>

        <Form.Select
          name="month"
          value={postFilter.monthList}
          onChange={(e) => dispatch(updateMonth(e.target.value))}
          title="Month"
        >
          <option value="">-- Chọn tháng --</option>
          {filter.monthList.length > 0 &&
            filter.monthList.map((item, index) => (
              <option key={index} value={item.value}>
                {item.text}
              </option>
            ))}
        </Form.Select>
      </Form.Group>

      <Form.Group className="col-auto">
        <Button variant="danger" type="submit">
          Tìm/lọc
        </Button>
        <Link to='/admin/posts/edit' className="btn btn-success ms-2">Thêm mới</Link>
      </Form.Group>
    </Form>
  );
};

export default PostFilerPane;