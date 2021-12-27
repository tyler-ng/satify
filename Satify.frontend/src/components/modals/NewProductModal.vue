<template>
  <satify-modal>
    <template v-slot:header> Add New Product </template>
    <template v-slot:body>
      <ul class="newProduct">
        <li>
          <label for="isTaxable">Is this product taxable?</label>
          <input
            type="checkbox"
            id="isTaxable"
            v-model="newProduct.isTaxable"
          />
        </li>
        <li>
          <label for="productName">Name</label>
          <input type="text" id="productName" v-model="newProduct.name" />
        </li>
        <li>
          <label for="productPrice">Price (CAD)</label>
          <input type="number" id="productPrice" v-model="newProduct.price" />
        </li>
        <li>
          <label for="idealQuantity">Ideal Quantity</label>
          <input
            type="number"
            id="idealQuantity"
            v-model="newProduct.idealQuantity"
          />
        </li>
        <li>
          <label>Description</label>
          <textarea id="productDesc" v-model="newProduct.description" />
        </li>
      </ul>
    </template>
    <template v-slot:footer>
      <satify-button
        type="button"
        @click.native="save"
        aria-label="save new item"
      >
        Save Product
      </satify-button>
      <satify-button
        type="button"
        @click.native="close"
        aria-label="close modal"
      >
        Close
      </satify-button>
    </template>
  </satify-modal>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { IProduct } from "@/types/Product";
import SatifyButton from "@/components/SatifyButton.vue";
import SatifyModal from "@/components/modals/SatifyModal.vue";

@Component({
  name: "NewProductModal",
  components: { SatifyButton, SatifyModal },
})
export default class NewProductModal extends Vue {
  newProduct: IProduct = {
    createdOn: new Date(),
    updatedOn: new Date(),
    id: 0,
    description: "",
    isTaxable: false,
    name: "",
    price: 0,
    isArchived: false,
    idealQuantity: 0,
  };

  close() {
    this.$emit("close");
  }

  save() {
    this.$emit("save:product", this.newProduct);
  }
}
</script>

<style scoped lang="scss">
.newProduct {
  list-style: none;
  padding: 0;
  margin: 0;
}
textarea {
  width: 100%;
  height: 200px;
  font-family: Avenir, Helvetica, Arial, sans-serif;
}
</style>
