<template>
  <div>
    <h1 id="newInvoiceTitle">Create Invoice</h1>
    <hr />
    <div class="invoice-step" v-if="invoiceStep === 1">
      <h2>Step 1: Select Customer</h2>
      <div v-if="customers.length" class="invoice-step-detail">
        <label for="customer">Customer:</label>
        <select
          v-model="selectedCustomerId"
          class="invoice-customers"
          id="customer"
        >
          <option disabled value="">Please select a Customer</option>
          <option v-for="c in customers" :value="c.id" :key="c.id">
            {{ c.firstName + " " + c.lastName }}
          </option>
        </select>
      </div>
    </div>
    <div class="invoice-step" v-if="invoiceStep === 2">
      <h2>Step 2: Create Order</h2>

      <div v-if="inventory.length" class="invoice-step-detail">
        <label for="product">Product:</label>
        <select v-model="newItem.product" class="invoiceLineItem" id="product">
          <option disabled value="">Please select a Product</option>
          <option v-for="i in inventory" :value="i.product" :key="i.product.id">
            {{ i.product.name }}
          </option>
        </select>
        <label for="quantity">Quantity</label>
        <input v-model="newItem.quantity" id="quantity" type="number" min="0" />
      </div>

      <div class="invoice-item-actions">
        <satify-button
          :disabled="!newItem.product || !newItem.quantity"
          @button:click="addLineItem"
        >
          Add Line Item
        </satify-button>
        <satify-button
          :disabled="!lineItems.length"
          @button:click="finalizeOrder"
        >
          Finalize Order
        </satify-button>
      </div>

      <div class="invoice-order-list" v-if="lineItems.length">
        <div class="runningTotal">
          <h3>Running Total:</h3>
          {{ runningTotal | price }}
        </div>
        <hr />
        <table class="table">
          <thead>
            <tr>
              <th>Product</th>
              <th>Qty. (bags)</th>
              <th>Price</th>
              <th>Subtotal</th>
            </tr>
          </thead>
          <tr
            v-for="lineItem in lineItems"
            :key="`index_${lineItem.product.id}`"
          >
            <td>{{ lineItem.product.name }}</td>
            <td>{{ lineItem.quantity }}</td>
            <td>{{ lineItem.product.price | price }}</td>
            <td>{{ (lineItem.product.price * lineItem.quantity) | price }}</td>
          </tr>
        </table>
      </div>
    </div>

    <div class="invoice-step" v-if="invoiceStep === 3">
      <h2>Step 3: Review and Submit</h2>
      <satify-button @button:click="submitInvoice"
        >Submit Invoice</satify-button
      >
      <hr />

      <div class="invoice-step-detail" id="invoice" ref="invoice">
        <div class="invoice-logo">
          <img
            id="imgLogo"
            alt="Tofu and Rolls logo"
            src="../assets/images/tofu_and_rolls_logo.png"
          />
          <h3>1923C Avenue Road</h3>
          <h3>North York, Ont</h3>
          <h3>647.532.8746</h3>
          <h3>www.tofuandrolls.com</h3>

          <div class="invoice-order-list" v-if="lineItems.length">
            <div class="invoice-header">
              <h3>Invoice: {{ new Date() | humanizeDate }}</h3>
              <h3>
                Customer:
                {{
                  this.selectedCustomer.firstName +
                  " " +
                  this.selectedCustomer.lastName
                }}
              </h3>
              <h3>
                Address: {{ this.selectedCustomer.primaryAddress.addressLine1 }}
              </h3>
              <h3>
                {{ this.selectedCustomer.primaryAddress.city }}
                {{ this.selectedCustomer.primaryAddress.province }}
                {{ this.selectedCustomer.primaryAddress.postalCode }}
              </h3>
              <h3>
                {{ this.selectedCustomer.primaryAddress.country }}
              </h3>
            </div>
            <table class="table">
              <thead>
                <tr>
                  <th>Product</th>
                  <th>Qty. (bags)</th>
                  <th>Price</th>
                  <th>Subtotal</th>
                </tr>
              </thead>
              <tr
                v-for="(lineItem, index) in lineItems"
                :key="`index_${lineItem.product.id}`"
              >
                <td>{{ lineItem.product.name }}</td>
                <td>{{ lineItem.quantity }}</td>
                <td>{{ lineItem.product.price | price }}</td>
                <td>
                  {{ (lineItem.product.price * lineItem.quantity) | price }}
                </td>
              </tr>
              <tr>
                <th colspan="3"></th>
                <th>Grand Total</th>
              </tr>
              <tfoot>
                <tr>
                  <td colspan="3" class="due">Balance due upon receipt:</td>
                  <td class="price-final">{{ runningTotal | price }}</td>
                </tr>
              </tfoot>
            </table>
          </div>
        </div>
      </div>
    </div>
    <hr />
    <div class="invoice-steps-actions">
      <satify-button @button:click="prev" :disabled="!canGoPrev"
        >Previous</satify-button
      >
      <satify-button @button:click="next" :disabled="!canGoNext"
        >Next</satify-button
      >
      <satify-button @button:click="startOver">Start Over</satify-button>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { IInvoice, ILineItem } from "@/types/Invoice";
import { ICustomer } from "@/types/Customer";
import { IProductInventory } from "@/types/Product";
import CustomerService from "@/services/customer-service";
import { InventoryService } from "@/services/inventory-service";
import InvoiceService from "@/services/invoice-service";
import SatifyButton from "@/components/SatifyButton.vue";
import jsPDF from "jspdf";
import html2canvas from "html2canvas";

const customerService = new CustomerService();
const inventoryService = new InventoryService();
const invoiceService = new InvoiceService();

@Component({
  name: "CreateInvoice",
  components: { SatifyButton },
})
export default class CreateInvoice extends Vue {
  $refs: {
    invoice: HTMLElement;
  };

  invoiceStep: number = 1;
  invoice: IInvoice = {
    customerId: 0,
    lineItems: [],
    createdOn: new Date(),
    updatedOn: new Date(),
  };
  customers: ICustomer[] = [];
  selectedCustomerId: number = 0;
  inventory: IProductInventory[] = [];
  lineItems: ILineItem[] = [];
  newItem: ILineItem = { product: undefined, quantity: 0 };

  get canGoNext() {
    if (this.invoiceStep === 1) {
      return this.selectedCustomerId !== 0;
    }

    if (this.invoiceStep === 2) {
      return this.lineItems.length;
    }

    if (this.invoiceStep === 3) {
      return false;
    }

    return false;
  }

  get canGoPrev() {
    return this.invoiceStep !== 1;
  }

  get runningTotal() {
    return this.lineItems.reduce(
      (a, b) => a + b["product"]!["price"] * b["quantity"],
      0
    );
  }

  get selectedCustomer() {
    return this.customers.find((c) => c.id == this.selectedCustomerId);
  }

  async submitInvoice(): Promise<void> {
    let now = new Date();
    this.invoice = {
      customerId: this.selectedCustomerId,
      lineItems: this.lineItems,
      createdOn: now,
      updatedOn: now,
    };

    await invoiceService.makeNewInvoice(this.invoice);
    this.downloadPdf();
    await this.$router.push("/orders");
  }

  downloadPdf() {
    let pdf = new jsPDF("p", "pt", "a4", true);
    let invoice = document.getElementById("invoice");
    let width = this.$refs.invoice.clientWidth;
    let height = this.$refs.invoice.clientHeight;

    html2canvas(invoice!).then((canvas) => {
      let image = canvas.toDataURL("image/png");
      pdf.addImage(image, "PNG", 0, 0, width * 0.45, height * 0.45);
      pdf.save("invoice");
    });
  }

  addLineItem() {
    let newItem: ILineItem = {
      product: this.newItem.product,
      quantity: Number(this.newItem.quantity.toString()),
    };

    let existingItems = this.lineItems.map((item) => item.product!.id);

    if (existingItems.includes(newItem.product!.id)) {
      let lineItem = this.lineItems.find(
        (item) => item.product!.id === newItem.product!.id
      );

      let currentQuantity = Number(lineItem!.quantity);
      lineItem!.quantity = currentQuantity += newItem.quantity;
    } else {
      this.lineItems.push(this.newItem);
    }

    this.newItem = { product: undefined, quantity: 0 };
  }

  finalizeOrder() {
    this.invoiceStep = 3;
  }

  startOver() {
    let now = new Date();
    this.invoice = {
      customerId: 0,
      lineItems: [],
      createdOn: now,
      updatedOn: now,
    };
    this.invoiceStep = 1;
  }

  prev(): void {
    if (this.invoiceStep === 1) {
      return;
    }
    this.invoiceStep -= 1;
  }

  next(): void {
    if (this.invoiceStep === 3) {
      return;
    }
    this.invoiceStep += 1;
  }

  async initialize(): Promise<void> {
    this.customers = await customerService.getCustomers();
    this.inventory = await inventoryService.getInventory();
  }

  async created() {
    await this.initialize();
  }
}
</script>

<style scoped lang="scss">
@import "@/scss/global.scss";
.invoice-steps-actions {
  display: flex;
  width: 100%;
}
.invoice-step {
}
.invoice-step-detail {
  margin: 1.2rem;
}
.invoice-order-list {
  margin-top: 1.2rem;
  padding: 0.8rem;
  .line-item {
    display: flex;
    border-bottom: 1px dashed #ccc;
    padding: 0.8rem;
  }
  .item-col {
    flex-grow: 1;
  }
}
.invoice-item-actions {
  display: flex;
}
.price-pre-tax {
  font-weight: bold;
}
.price-final {
  font-weight: bold;
  color: $satify-green;
}
.due {
  font-weight: bold;
}
.invoice-header {
  width: 100%;
  margin-bottom: 1.2rem;
}
.invoice-logo {
  padding-top: 1.4rem;
  text-align: center;
  img {
    width: 280px;
  }
}
</style>
