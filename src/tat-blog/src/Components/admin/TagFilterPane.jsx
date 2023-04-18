import { useState, useEffect } from "react";
import { Button, Form } from "react-bootstrap";

import { getFilterTag } from "../../Services/TagCloudWidget";

import { reset, updateKeyword } from '../../Redux/Reducer';
import { useDispatch, useSelector } from "react-redux";
import { Link } from "react-router-dom";


const TagFilterPane = () => {

  const tagsFilter = useSelector(state => state.tagsFilter), dispatch = useDispatch(),
  [filter, setFilter] = useState();

  const handleReset = (e) => {
    dispatch(reset());
  };

  const handleSubmit = (e) => {
    e.preventDefault();
  };

  useEffect(() => {
    getFilterTag().then((data) => {
      if(data){
        console.log("data: ", data);
        setFilter(data);

      }
      else{
        setFilter([]);
      }
    } )
  }, [])

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
          name="name"
          value={tagsFilter}
          onChange={(e) => dispatch(updateKeyword(e.target.value))}
        />
      </Form.Group>

      <Form.Group className="col-auto">
        <Button variant="danger" type="submit">
          Xoá Lọc
        </Button>
        <Button  type="submit" className="btn btn-primary ms-2">
          Bỏ lọc
        </Button>
        <Link to='/admin/posts/edit' className="btn btn-success ms-2">Thêm mới</Link>
      </Form.Group>
    </Form>
  );
}

export default TagFilterPane;